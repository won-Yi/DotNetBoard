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

namespace Board.Controllers
{

    using Board.Models;

    public class NoticeDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }

        public string UserName { get; set; }

        public DateTime UpdateDate { get; set; }

        public List<Comments> comments { get; set; }
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
        public async Task<IActionResult> Index()
        {

            Notice notice = new Notice();
            var notice_list = Enumerable.Reverse(_context.Notice).ToList();


            return View(notice_list);

            // ToListAsync().설명.. List<T> 비동기적으로 열거하여 A를 IQueryable 만듭니다.
            // 결국 이게 _context를 통해 Notice Model에 연결되어 ToListAsync()함수를 통해 목록 list가 나온다는 이야기.
        }

        public class DtoClass{
            List<Notice> notice = new List<Notice>();
            List<Comments> comments = new List<Comments>();
        }

        // GET: Notices/Details/5
        public async Task<IActionResult> Details(int? id, [Bind("Id,Comment,UserName")] Notice notice)
        {


            if (id == null || _context.Notice == null)
            {
                return NotFound();
            }

            await _context.Notice
                .FirstOrDefaultAsync(m => m.Id == id);





            //FirstOrDefaultAsync()는 시퀀스의 첫 번째 요소를 비동기적으로 반환하거나,
            //시퀀스에 요소가 없는 경우 기본값을 반환합니다.
            //(m => m.Id == id)에서 m은 어디서 설정이 안되어 있지만 모델로 간주.

            //게시글에 달린 댓글목록을 가져와야한다.

            //var comment = _context.Comments.FirstOrDefault(m => m.Notice_id == id);

            //if ( comment == null) {
            //    return View(notice);
            //}

            //ViewBag.username = comment.UserName;
            //ViewBag.comment = comment.Comment;
            //


            //var dto = new BookDto()
            //{
            //    Id = book.Id,
            //    Title = book.Title,
            //    AuthorName = book.Author.Name
            //Notice List()ViewNotice { get; private set; }
            //Notice ViewNotice { get; private set; }
            //};

            var comment = from m in _context.Comments select m;
            comment = comment.Where(s => s.Notice_id==id);

           
            


//ModelView를 가지고 놀아볼 시간 Let's begin!

if (notice == null)
            {
                return NotFound();
            }

            return View(notice);
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
        public async Task<IActionResult> Create([Bind("Id,Title,Content,UserName")] Notice notice)
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
            _context.Notice.FindAsync(id);
           

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


        //Comment
        [HttpPost, ActionName("CommentCreate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CommentCreate(int? id , string Username, string Comment)
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
                return RedirectToAction(nameof(Index));
            }

            Debug.WriteLine("여기가 실행되는거지?");
            return View();
        }


        private bool NoticeExists(int id)
        {
          return (_context.Notice?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }//
}//
