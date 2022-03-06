import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BlogsComponent } from './blogs.component';
import { AddBlogComponent } from './add-blog/add-blog.component';
import { EditBlogComponent } from './edit-blog/edit-blog.component';
import { SharedModule } from '../shared/shared.module';
import { BlogsRoutingModule } from './blogs-routing.module';
import { ListBlogsAdminComponent } from './list-blogs-admin/list-blogs-admin.component';
import { BlogComponent } from './blog/blog.component';
import { InfoBlogComponent } from './info-blog/info-blog.component';
import { CommentFunctionalityComponent } from './comment-functionality/comment-functionality.component';
import { CommentsComponent } from './comments/comments.component';
import { CommentBoxComponent } from './comment-box/comment-box.component';



@NgModule({
  declarations: [
    BlogsComponent,
    AddBlogComponent,
    EditBlogComponent,
    ListBlogsAdminComponent,
    BlogComponent,
    InfoBlogComponent,
    CommentFunctionalityComponent,
    CommentsComponent,
    CommentBoxComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    BlogsRoutingModule
  ]
})
export class BlogsModule { }
