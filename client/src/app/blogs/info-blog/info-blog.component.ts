import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Blog } from 'src/app/shared/models/blog';
import { BlogsService } from '../blogs.service';

@Component({
  selector: 'app-info-blog',
  templateUrl: './info-blog.component.html',
  styleUrls: ['./info-blog.component.css']
})
export class InfoBlogComponent implements OnInit {
  blog: Blog;

  constructor(private blogsService: BlogsService,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadBlogs();
  }

  loadBlogs() {
    return this.blogsService.getBlogById(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe(response => {
    this.blog = response;
    }, error => {
    console.log(error);
    });
    }

}
