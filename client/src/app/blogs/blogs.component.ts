import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Blog } from '../shared/models/blog';
import { UserParams } from '../shared/models/myparams';
import { BlogsService } from './blogs.service';

@Component({
  selector: 'app-blogs',
  templateUrl: './blogs.component.html',
  styleUrls: ['./blogs.component.css']
})
export class BlogsComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  blogs: Blog[];
  userParams: UserParams;
  totalCount: number;

  constructor(private blogsService: BlogsService,
              private  router: Router,
              private toastr: ToastrService) {
    this.userParams = this.blogsService.getUserParams();
     }

  ngOnInit(): void {
    this.getBlogs();
  }

  getBlogs() {
    this.blogsService.setUserParams(this.userParams);
    this.blogsService.getBlogs(this.userParams)
    .subscribe(response => {
      this.blogs = response.data;
      this.userParams.page = response.page;
      this.userParams.pageCount = response.pageCount;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    }
    );
  }

  onSearch() {
    this.userParams.query = this.searchTerm.nativeElement.value;
    this.getBlogs();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.blogsService.resetUserParams();
    this.getBlogs();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.blogsService.setUserParams(this.userParams);
      this.getBlogs();
    }
}

}
