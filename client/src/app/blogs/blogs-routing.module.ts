import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { BlogsComponent } from './blogs.component';
import { AddBlogComponent } from './add-blog/add-blog.component';
import { EditBlogComponent } from './edit-blog/edit-blog.component';
import { ListBlogsAdminComponent } from './list-blogs-admin/list-blogs-admin.component';
import { InfoBlogComponent } from './info-blog/info-blog.component';

const routes: Routes = [
  {path: '', component: BlogsComponent},
  {path: 'bloglist', component: ListBlogsAdminComponent},
  {path: 'addblog', component: AddBlogComponent},
  {path: 'infoblog/:id', component: InfoBlogComponent},
  {path: 'editblog/:id', component: EditBlogComponent},

];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class BlogsRoutingModule { }
