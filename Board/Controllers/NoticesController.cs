using System;
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

namespace Board.Controllers
{

    using Board.Models;

    public class NoticeDto
    {
        public int Id { get; set; }

        public int Views_Number { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }

        public string UserName { get; set; }

        public DateTime UpdateDate { get; set; }

        public List<Comments> Comments { get; set; }
       
        public string? Category { get; set; }
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

            //LINQ to get list of category
            IQueryable<string> categoryQuery = from m in _context.Notice
                                               select m.Category;

            var notice = from m in _context.Notice select m;
           

            if (!string.IsNullOrEmpty(searchString)) {
                notice = notice.Where(x => x.Title!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(Category))
            {
                notice = notice.Where(x => x.Category == Category);
              
            }
            var noticeCategory = new NoticeCategory
            {
                Categorys = new SelectList(await categoryQuery.Distinct().ToListAsync()),
                Notices = await notice.ToListAsync()
                

            };
           
            //var notice_list = Enumerable.Reverse(_context.Notice).ToList();

            return View(noticeCategory);
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

            //var comment = from m in _context.Comments select m;
            //comment = comment.Where(s => s.Notice_id == id);


            var comment = from m in _context.Comments where m.Notice_id == id select m;



            var notice_dto = new NoticeDto()
            {
                Id = notice.Id,
                UserName = notice.UserName,
                Content = notice.Content,
                Title = notice.Title,
                UpdateDate = notice.UpdateDate,
                Views_Number = notice.Views_Number,

                Comments = comment.ToList(),
               
            };
            
            
            //ModelView를 가지고 놀아볼 시간 Let's begin!

            if (notice_dto == null)
            {
                return NotFound();
            }

            return View(notice_dto);
        }

        // GET: Notices/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Notices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,UserName,Category")] Notice notice)
        {
            if (ModelState.IsValid)
            {

                //현재 시간을 데이터 베이스에 넣어준다.
                DateTime time_now = DateTime.Now;
                notice.UpdateDate = time_now;
            
                _context.Add(notice);
                await _context.SaveChangesAsync();
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,UserName")] Notice notice)
        {


            if (id != notice.Id)
            {
                return NotFound();
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

        // GET: Notices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Notice == null)
            {
                return NotFound();
            }

            var notice = await _context.Notice
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notice == null)
            {
                return NotFound();
            }

            return View(notice);
        }

        // POST: Notices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Notice == null)
            {
                return Problem("Entity set 'BoardContext.Notice'  is null.");
            }
            var notice = await _context.Notice.FindAsync(id);
            if (notice != null)
            {
                _context.Notice.Remove(notice);
            }
            
            await _context.SaveChangesAsync();
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

            Debug.WriteLine("여기가 실행되는거지?");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CommentEdit(int? id, int Id, string UserName, string editComment )
        {


            Comments comments = new Comments();
            comments.Notice_id = (int)id;
            comments.UserName = (string)UserName;   
            comments.Comment = editComment;
            comments.Id = (int)Id;



            if (ModelState.IsValid)
            {
                try
                {
                    DateTime time_now = DateTime.Now;
                    comments.UpdateTime = time_now;

                    _context.Update(comments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoticeExists(comments.Id))
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
        public async Task<IActionResult> CommentDelete(int? commentId)
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
            return RedirectToAction(nameof(Index));
        }


        private bool NoticeExists(int id)
        {
          return (_context.Notice?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }//
}//
