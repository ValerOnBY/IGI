﻿using Infastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Weapon_Shop.Feature.Weapon
{
    public class WeaponController: Controller
    {
        private readonly IMediator _mediator;
        private readonly AppIdentityDbContext _context;
        public WeaponController(IMediator mediator, AppIdentityDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("Cart") != null)
               ViewBag.Weapons = JsonSerializer.Deserialize<List<Infastructure.Entities.Weapon>>(HttpContext.Session.GetString("Cart")).Select(x=>x.Id);
            
            return View(_context.Weapon.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Create.Command command)
        {
            await _mediator.Send(command);
            return RedirectToAction("Index","Weapon");
        }

        public async Task<IActionResult> Delete(Delete.Command command)
        {
            await _mediator.Send(command);
            return RedirectToAction("Index");
        }
    }
}
