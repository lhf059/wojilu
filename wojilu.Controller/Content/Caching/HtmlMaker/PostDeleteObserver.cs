﻿using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using System.IO;
using wojilu.Web.Context;
using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Apps.Content.Service;

namespace wojilu.Web.Controller.Content.Caching.Actions {

    public class PostDeleteObserver : ActionObserver {

        public IContentPostService postService { get; set; }

        public PostDeleteObserver() {
            postService = new ContentPostService();
        }

        public override void ObserveActions() {

            Admin.Common.PostController post = new wojilu.Web.Controller.Content.Admin.Common.PostController();
            observe( post.Delete );
            observe( post.DeleteSys );

            Admin.Section.TalkController talk = new wojilu.Web.Controller.Content.Admin.Section.TalkController();
            observe( talk.Delete );

            Admin.Section.TextController txt = new wojilu.Web.Controller.Content.Admin.Section.TextController();
            observe( txt.Delete );

            Admin.Section.VideoController video = new wojilu.Web.Controller.Content.Admin.Section.VideoController();
            observe( video.Delete );

            Admin.Section.ImgController img = new wojilu.Web.Controller.Content.Admin.Section.ImgController();
            observe( img.Delete );

            Admin.Common.PollController poll = new wojilu.Web.Controller.Content.Admin.Common.PollController();
            observe( poll.Delete );
        }

        private ContentPost _contentPost;


        public override bool BeforeAction( MvcContext ctx ) {

            ContentPost post = postService.GetById( ctx.route.id, ctx.owner.Id );
            _contentPost = post;

            return base.BeforeAction( ctx );
        }

        public override void AfterAction( Context.MvcContext ctx ) {

            // 1）文章删除之后，app首页和侧边栏都要更新
            new HomeMaker( ctx ).Process( ctx.app.Id );
            new SidebarMaker( ctx ).Process( ctx.app.Id );

            // 2）删除文章详细页
            new DetailMaker( ctx ).Delete( _contentPost );

            // 3）更新列表页
            new ListMaker( ctx ).Process( _contentPost );

        }


    }

}
