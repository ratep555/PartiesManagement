import { Component, OnInit } from '@angular/core';
import { Blog } from 'src/app/shared/models/blog';
import { MyParams } from 'src/app/shared/models/myparams';
import { BlogsService } from '../blogs.service';

@Component({
  selector: 'app-list-blogs-admin',
  templateUrl: './list-blogs-admin.component.html',
  styleUrls: ['./list-blogs-admin.component.css']
})
export class ListBlogsAdminComponent implements OnInit {
  blogs: Blog[];
  myParams: MyParams;
  totalCount: number;

  constructor(private blogsService: BlogsService) {
    this.myParams = this.blogsService.getMyParams();
   }

  ngOnInit(): void {
    this.getBlogs();
  }

  getBlogs() {
    this.blogsService.getAllBlogs().subscribe(response => {
      this.blogs = response.data;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    });
  }

  onPageChanged(event: any) {
    const params = this.blogsService.getMyParams();
    if (params.page !== event) {
      params.page = event;
      this.blogsService.setMyParams(params);
      this.getBlogs();
    }
  }


}
