using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Board.Data;
using Board.Models;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using System.Xml.Linq;
using Azure.Identity;
using Board.Migrations;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.Identity.Client;
using System.Reflection;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
using static Dropbox.Api.Files.ListRevisionsMode;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Dropbox.Api.Sharing.ListFileMembersIndividualResult;

namespace Board.Controllers
{

    using Board.Models;
    using System.Reflection.PortableExecutable;

    public class NoticeDto
    {
        public int Id { get; set; }

        public int Views_Number { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }

        public string UserName { get; set; }

        public int? LikeNotice { get; set; }

        public DateTime UpdateDate { get; set; }

        public List<Comments> Comments { get; set; }

        public List<FileModel> FileModel { get; set; }
        public string Category { get; set; }

        public string? FileName { get; set; }
        public string? fileAttachMent { get; set; }

        public List<Notice>? Notices { get; set; }
        public SelectList? Categorys { get; set; }

        public string? SearchString { get; set; }

        //string[]? 이렇게 하면 하나의 값만 가져온다
        //Notice[]?로 해야 Notice의 객체는 게시물에 대한 정보를 담고 있으므로
        //정보를 다 가져올 수 있다..
        public Notice[]? BestNotice { get; set; }

    }


    public class NoticesController : Controller
    {

        //context 
        private readonly BoardContext _context;

        public NoticesController(BoardContext context)
        {
            _context = context;
        }


        // GET: Notices
        [ActionName("Index")]
        public async Task<IActionResult> Index(string? Category, string? searchString)
        {

           //LINQ 
            IQueryable<string> categoryQuery = from m in _context.Notice
                                               orderby m.UpdateDate descending
                                               select m.Category;

            var notice = from m in _context.Notice select m;
            //게시글 역순으로 불러오기 위한 orderby m.UpdateDate descending 추가
            //var notice_list = Enumerable.Reverse(_context.Notice).ToList(); 전에는 Reverse를 사용했었다.
            notice = from m in _context.Notice orderby m.UpdateDate descending select m;

            //take를 사용하면 값 몇개만 선택해서 가져오기 가능.
            var notice_best = await _context.Notice
                                    .Where(m => m.LikeNotice.HasValue)
                                    .OrderByDescending(m => m.LikeNotice.Value)
                                    .Take(3)
                                    .ToArrayAsync();

            //DTO에 notice_best를 넣기 위해 문자열로 변경하여 새로운 변수에 저장한다..?



            if (!string.IsNullOrEmpty(searchString)) {

                notice = notice.Where(x => x.Title!.Contains(searchString)); 
            }

            if (!string.IsNullOrEmpty(Category))
            {
               
                notice = notice.Where(x => x.Category == Category);
              
            }

            //이 부분은 아직 이해를 잘 하지 못함.
            var noticeCategory = new NoticeCategory
            {
                Categorys = new SelectList(await categoryQuery.Distinct().ToListAsync()),
                Notices = await notice.ToListAsync(),
            };
            //이걸 noticeDTO로 새롭게 만들어서 noticeCategory저거 자체를 DTO에 넣고 보낼 수 있을까?

            var NoticeDto = new NoticeDto
            {
                Categorys = noticeCategory.Categorys,
                Notices = noticeCategory.Notices,
                BestNotice = notice_best,
            };

            return View(NoticeDto);
        }

        // GET: Notices/Details/5
        public async Task<IActionResult> Details(int? id, [Bind("Id,Comment,UserName")] Notice notice)
        {

            if (id == null || _context.Notice == null)
            {
                return NotFound();
            }
                notice = await _context.Notice.FirstOrDefaultAsync(m => m.Id == id);
                //조회수 추가
                notice.Views_Number++;
                await _context.SaveChangesAsync();
            
            var comment = from m in _context.Comments where m.Notice_id == id select m;

            var filemodel = from m in _context.FileModel where m.NoticeId == id select m;

            var notice_dto = new NoticeDto()
            {
                Id = notice.Id,
                UserName = notice.UserName,
                Content = notice.Content,
                Title = notice.Title,
                UpdateDate = notice.UpdateDate,
                Views_Number = notice.Views_Number,
                FileModel = filemodel.ToList(),
                Comments = comment.ToList(),
            };
            
            if (notice_dto == null)
            {
                return NotFound();
            }

            return View(notice_dto);
        }


        //게시글 좋아요
        public async Task<IActionResult> Likes(int? Id)
        {

            Notice notice = new Notice();
            notice =  await _context.Notice.FirstOrDefaultAsync(m => m.Id == Id);

            notice.LikeNotice++;
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Notices", new { id = Id });
        }


        //파일 다운로드
        public FileResult FileDownload(string? filename) {

            FileModel fileModel = new FileModel();
            fileModel = _context.FileModel.FirstOrDefault(m => m.FileNames == filename);


            string filepath = fileModel.FilePath;        // 파일 경로
            string filenames = fileModel.FileNames;      // 파일명
            string path = filepath;  // 파일 경로 / 파일명
               
            
                
            // 파일을 바이트 형식으로 읽음
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            // 파일 다운로드 처리
            return File(bytes, "application/octet-stream", filenames);
        }



        // GET: Notices/Create
        public IActionResult Create() {
           
            IQueryable<string> categoryQuery = from m in _context.Notice
                                               select m.Category;

            var Categorys = new SelectList(categoryQuery.ToList().Distinct());
            ViewBag.Categories = Categorys;

            return View();
        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<IFormFile> files, [Bind("Id", "Title", "Content", "UserName", "Category")] Notice notice)
        {
            int result = -1;

            if (ModelState.IsValid)
            {
                //현재 시간을 데이터 베이스에 넣어준다.
                DateTime time_now = DateTime.Now;
                notice.UpdateDate = time_now;

                _context.Add(notice);
                await _context.SaveChangesAsync();

                string uploadDir = "D:/code/Board/UploadPath/";
                try
                {
                    // 업로드 폴더 경로 존재 확인
                    DirectoryInfo di = new DirectoryInfo(uploadDir);
                    // 폴더가 없을 경우 신규 작성
                    if (di.Exists == false) di.Create();

                    // 선택한 파일 개수만큼 반복
                    foreach (var formFile in files)
                    {
                        if (formFile.Length > 0)
                        {
                            var fileFullPath = uploadDir + formFile.FileName;

                            // 파일명이 이미 존재하는 경우 파일명 변경
                            int filecnt = 1;
                            System.String newFilename = string.Empty;
                            while (new FileInfo(fileFullPath).Exists)
                            {
                                var idx = formFile.FileName.LastIndexOf('.');
                                var tmp = formFile.FileName.Substring(0, idx);
                                newFilename = tmp + System.String.Format("({0})", filecnt++) + formFile.FileName.Substring(idx);
                                fileFullPath = uploadDir + newFilename;
                            }

                           

                            FileModel model = new FileModel();

                            model.NoticeId = notice.Id;
                            model.FilePath = fileFullPath;
                            model.FileNames = formFile.FileName;
                            _context.Add(model);
                            await _context.SaveChangesAsync();

                            // 파일 업로드
                            using (var stream = new FileStream(fileFullPath, FileMode.CreateNew))
                            {
                                await formFile.CopyToAsync(stream);
                            }
                        }
                    }
                    result = 0;
                }
                catch (Exception)
                {
                    throw;
                }
                //Notice notice = new Notice();
                return RedirectToAction(nameof(Index));
            }
            

            return View(notice);
        }

        // GET: Notices/Edit/5
        public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Notice == null)
        {
            return NotFound();
        }
        Notice notice = new Notice();           
        notice = _context.Notice.FirstOrDefault(x => x.Id == id);
            
        if (notice == null)
        {
            return NotFound();
        }
        return View(notice);
    }

        // POST: Notices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, List<IFormFile> files, [Bind("Id,Title,Content,UserName,Category")] Notice notice)
        {

            int result = -1;
            if (id != notice.Id)
            {
                return NotFound();
            }

            var fileModels = _context.FileModel.Where(x => x.NoticeId == notice.Id).ToList();
            if (files != null && files.Count > 0)
            {
                    _context.FileModel.RemoveRange(fileModels);
                    await _context.SaveChangesAsync();
                    result = 1;
            }

            string uploadDir = "D:/code/Board/UploadPath/";
            try
            {
                // 업로드 폴더 경로 존재 확인
                DirectoryInfo di = new DirectoryInfo(uploadDir);
                // 폴더가 없을 경우 신규 작성
                if (di.Exists == false) di.Create();

                // 선택한 파일 개수만큼 반복
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        var fileFullPath = uploadDir + formFile.FileName;

                        // 파일명이 이미 존재하는 경우 파일명 변경
                        int filecnt = 1;
                        System.String newFilename = string.Empty;
                        while (new FileInfo(fileFullPath).Exists)
                        {
                            var idx = formFile.FileName.LastIndexOf('.');
                            var tmp = formFile.FileName.Substring(0, idx);
                            newFilename = tmp + System.String.Format("({0})", filecnt++) + formFile.FileName.Substring(idx);
                            fileFullPath = uploadDir + newFilename;
                        }

                        FileModel model = new FileModel();

                        model.NoticeId = notice.Id;
                        model.FilePath = fileFullPath;
                        model.FileNames = formFile.FileName;
                        _context.Add(model);
                        await _context.SaveChangesAsync();


                        // 파일 업로드
                        using (var stream = new FileStream(fileFullPath, FileMode.CreateNew))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                }
                result = 0;
            }
            catch (Exception)
            {
                throw;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    DateTime time_now = DateTime.Now;
                    notice.UpdateDate = time_now;

                    _context.Update(notice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoticeExists(notice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(notice);
        }

        // POST: Notices/Delete/5
        //ActionName("DeleteConfirmed")
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Notice == null)
            {
                return Problem("Entity set 'BoardContext.Notice' is null.");
            }
            var notice = await _context.Notice.FindAsync(id);
           
            _context.Notice.Remove(notice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        //Bulk Delete
        [HttpPost]
        public async Task<IActionResult> BulkDelete(List<string>? ids) {
            
            Notice notice = new Notice();          
            foreach(var id in ids) { 
                var Id = int.Parse(id);
                var content = await _context.Notice.FindAsync(Id);
                _context.Notice.Remove(content);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost, ActionName("CommentCreate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CommentCreate(int? id, string? Username, string? Comment)
        {

            if (ModelState.IsValid)
            {
                Comments comments = new Comments();

                //View에서 ViewData를 사용해서 받아온 값
                ViewData["username"] = Username;
                ViewData["comment"] = Comment;

                //데이터 베이스에 넣기 위해 변수를 선언해 주었다.
                var username = ViewData["username"];
                var comment = ViewData["comment"];

                // comment모델에 값을 추가해준다.
                comments.Notice_id = (int)id;
                comments.UserName = (string)username;
                comments.Comment = (string)comment;


                //현재 시간을 데이터 베이스에 넣어준다.
                DateTime time_now = DateTime.Now;
                comments.UpdateTime = time_now;
                Debug.WriteLine(comments);

                _context.Add(comments);
                await _context.SaveChangesAsync();
                Response.Redirect("Details/" + id);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CommentEdit(int Id, string UserName, string editComment )
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id == Id);

            comment.UserName = (string)UserName;
            comment.Comment = editComment;
            comment.Id = (int)Id;

            if (ModelState.IsValid)
            {
                try
                {
                    DateTime time_now = DateTime.Now;
                    comment.UpdateTime = time_now;

                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoticeExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CommentDelete(int? commentId, [Bind("Id")] Notice notice)
        {
            if (_context.Notice == null)
            {
                return Problem("Entity set 'BoardContext.Notice'  is null.");
            }
            var comment = await _context.Comments.FindAsync(commentId);
            
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }

            await _context.SaveChangesAsync();
            if (commentId == null)
            {
                return RedirectToAction("Index", "Notices");
            }
            else
            {
                return RedirectToAction("Details", "Notices", new { id = notice.Id });
            }
        }

        private bool NoticeExists(int id)
        {
          return (_context.Notice?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
