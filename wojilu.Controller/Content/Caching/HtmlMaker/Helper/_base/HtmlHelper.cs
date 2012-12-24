﻿using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Apps.Content.Domain;
using System.IO;
using wojilu.Web.Context;
using wojilu.Web.Mvc;
using wojilu.ORM;
using wojilu.Members.Sites.Domain;

namespace wojilu.Web.Controller.Content.Caching {

    public class HtmlHelper {

        private static readonly ILog logger = LogManager.GetLogger( typeof( HtmlHelper ) );

        public static void SetPostToContext( MvcContext ctx, ContentPost post ) {
            ctx.SetItem( "_currentContentPost", post );
        }

        public static ContentPost GetPostFromContext( MvcContext ctx ) {
            return ctx.GetItem( "_currentContentPost" ) as ContentPost;
        }

        public static Boolean IsMakeHtml( MvcContext ctx ) {
            if (ctx.GetItem( "_makeHtml" ) == null) return false;
            Boolean isMakeHtml = (Boolean)ctx.GetItem( "_makeHtml" );
            return isMakeHtml;
        }

        public static bool IsHtmlDirError( String htmlDir, Result errors ) {

            if (strUtil.HasText( htmlDir )) {


                if (htmlDir.Length > 50) {
                    errors.Add( "目录名称不能超过50个字符" );
                    return true;
                }

                if (isReservedKeyContains( htmlDir )) {
                    errors.Add( "目录名称是保留词，请换一个" );
                    return true;
                }

                if (isHtmlDirUsed( htmlDir )) {
                    errors.Add( "目录名称已被使用，请换一个" );
                    return true;
                }

            }

            return false;
        }

        private static bool isHtmlDirUsed( string dirName ) {

            List<ContentApp> appList = ContentApp.find( "OwnerType=:otype" )
                .set( "otype", typeof( Site ).FullName )
                .list();

            foreach (ContentApp app in appList) {

                if (dirName.Equals( app.GetSettingsObj().StaticDir )) return true;
            }

            return false;
        }

        private static bool isReservedKeyContains( string dirName ) {

            if (dirName == null) return false;

            String[] arrKeys = new String[] { "framework", "bin", "html", "static" };

            return new List<String>( arrKeys ).Contains( dirName.ToLower() );
        }


    }

}
