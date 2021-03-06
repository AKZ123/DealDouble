﻿using DeakDouble.Services;
using DealDouble.Entities;
using DealDouble.Web.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DealDouble.Web.Controllers
{
    public class SharedController : Controller
    {
        SharedService service = new SharedService();

        [HttpPost]
        public JsonResult UploadPictures()
        {
            JsonResult result = new JsonResult();

            List<object> picturesJSON = new List<object>();

            var pictures = Request.Files;
            //foreach loop is not work here.
            for (int i = 0; i < pictures.Count; i++)
            {
                var picture = pictures[i];
                //                             Picture  Exrension .Jpg .PNG          picture real Name
                var fileName = Guid.NewGuid()+ Path.GetExtension(picture.FileName); //picture.FileName;
                var path = Server.MapPath("~/Content/images/") + fileName;
                picture.SaveAs(path);

                var dbPicture = new Picture();
                dbPicture.URL = fileName; //path;

                int pictureID = service.SavePicture(dbPicture);

                picturesJSON.Add(new { ID = pictureID, pictureURL = fileName }); // (pictureURL = path) send korlay Browser server ar path access koray na.
            }

            result.Data = picturesJSON;
            return result;
        }


        [HttpPost]
        public JsonResult LeaveComment(CommentViewModel model)
        {
            JsonResult result = new JsonResult();

            try
            {
                var comment = new Comment();

                comment.Text = model.Text;
                comment.Rating = model.Rating;
                comment.EntityID = model.EntityID;
                comment.RecordID = model.RecordID;

                comment.UserID = User.Identity.GetUserId();
                comment.TimeStamp = DateTime.Now;


                result.Data = new { Success = service.LeaveComment(comment) };
            }
            catch(Exception ex)
            {
                result.Data = new { Success = false, Message=ex.Message };
            }

            return result;
        }
    }
}