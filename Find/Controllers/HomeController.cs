using Find.Data;
using Find.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Find.Controllers
{
    public class HomeController : Controller
         
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
    }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Home/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Home/Create")]
        public async Task<IActionResult> Create([Bind("Id,Image,Name,Surname,Age,Gender,BirthDate,Email,Adress,Phone,MilitaryStatus,MaritalStatus,Experience,EducationalStatus,GraduationScore,Language,LanguageLevel,Reference,Explanation,Profession")] Candidate candidate, IFormFile? formFile)
        {

            if (ModelState.IsValid)
            {
				if (formFile != null)
				{

					Console.WriteLine("döngüye girdi");
					var extent = Path.GetExtension(formFile.FileName);
					var randomName = ($"{Guid.NewGuid()}{extent}");
					var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\assets\\uploads", randomName);
					Console.WriteLine(path);
					candidate.Image = randomName;

					using (var stream = new FileStream(path, FileMode.Create))
					{
						await formFile.CopyToAsync(stream);
					}
				}

					_context.Add(candidate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(candidate);
        }

        [Route("Home/Getlist")]
        public async Task<IActionResult> Getlist()
        {
            return View(await _context.CandidateList.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidate = await _context.CandidateList
                .FirstOrDefaultAsync(m => m.Id == id);
            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Details(int id, [Bind("Id,Image,Name,Surname,Age,Gender,BirthDate,Email,Adress,Phone,MilitaryStatus,MaritalStatus,Experience,EducationalStatus,GraduationScore,Language,LanguageLevel,Reference,Explanation,Profession")] Candidate candidate)
		{

			if (id != candidate.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
                    
					_context.Update(candidate);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CandidateExists(candidate.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return View(candidate);
			}
			return View(candidate);
		}

		[HttpPost]
        [Route("Home/Getlist")]
        public async Task<IActionResult> Getlist(int a, int b, int c, int d, int Age, string Gender, string MilitaryStatus, string Experience, string EducationalStatus, string Language, string Profession)
        {
			if ((a + b + c + d) != 100 && (a + b + c + d) != 0)
			{
				ViewData["error"] = "Hata: Deðerlerin toplamý 100 olmalýdýr.";
				return View(await _context.CandidateList.ToListAsync());
			}

			if (ModelState.IsValid)
			{
				var FiltredList = _context.CandidateList.Where(e => e.Id >= 0);

					if (MilitaryStatus != "Askerlik Durumu")
						FiltredList = FiltredList.Where(e => e.MilitaryStatus == MilitaryStatus);

					if (Experience != "Tecrübe")
						FiltredList = FiltredList.Where(e => e.Experience == Experience);

					if (EducationalStatus != "Eðitim Seviyesi")
						FiltredList = FiltredList.Where(e => e.EducationalStatus == EducationalStatus);

					if (Language != "Yabancý Dil")
						FiltredList = FiltredList.Where(e => e.Language == Language);

					if (Gender != "Cinsiyet")
						FiltredList = FiltredList.Where(e => e.Gender == Gender);

					if (Age != 0)
					{

						if (Age == 1)
							FiltredList = FiltredList.Where(e => e.Age >= 18 && e.Age < 25);
						if (Age == 2)
							FiltredList = FiltredList.Where(e => e.Age >= 25 && e.Age < 35);
						if (Age == 3)
							FiltredList = FiltredList.Where(e => e.Age >= 35 && e.Age <= 50);

					}

					if (Profession != "Meslek")
						FiltredList = FiltredList.Where(e => e.Profession == Profession);


					foreach (var item in FiltredList)
					{
						int rate;

						switch (item.EducationalStatus)
						{
							case "yüksek lisans":
								rate = a * 5;
								break;
							case "lisans":
								rate = a * 4;
								break;
							case "önlisans":
								rate = a * 3;
								break;
							case "lise":
								rate = a * 2;
								break;
							default:
								rate = a * 1;
								break;
						}
						switch (item.Experience)
						{
							case "10+ yýl":
								rate += b * 5;
								break;
							case "5-10 yýl":
								rate += b * 4;
								break;
							case "3-5 yýl":
								rate += b * 3;
								break;
							case "1-3 yýl":
								rate += b * 2;
								break;
							default:
								rate += b * 1;
								break;
						}
						switch (item.LanguageLevel)
						{
							case "B2":
								rate += c * 5;
								break;
							case "B1":
								rate += c * 4;
								break;
							case "A2":
								rate += c * 3;
								break;
							case "A1":
								rate += c * 2;
								break;
							default:
								rate += c * 1;
								break;
						}
						if (item.GraduationScore >= 3.5) rate += d * 5;
						else if (item.GraduationScore >= 3) rate += d * 4;
						else if (item.GraduationScore >= 2) rate += d * 3;
						else if (item.GraduationScore >= 1) rate += d * 2;
						else rate += d * 1;

						item.Rate = rate;

						Console.WriteLine(a);
						Console.WriteLine(b);
						Console.WriteLine(c);
						Console.WriteLine(d);
						Console.WriteLine(rate);
					}

					ViewData["status"] = "filtred";
					ViewData["a"] = a;
					ViewData["b"] = b;
					ViewData["c"] = c;
					ViewData["d"] = d;
					ViewData["EducationalStatus"] = EducationalStatus;
					ViewData["MilitaryStatus"] = MilitaryStatus;
					ViewData["Experience"] = Experience;
					ViewData["Age"] = Age;
					ViewData["Gender"] = Gender;
					ViewData["Profession"] = Profession;
					ViewData["Language"] = Language;

					FiltredList = FiltredList.OrderByDescending(e => e.Rate);
					return View(await FiltredList.ToListAsync());

				}
			
			return View(await _context.CandidateList.ToListAsync());
		}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
		private bool CandidateExists(int id)
		{
			return _context.CandidateList.Any(e => e.Id == id);
		}
	}
}
