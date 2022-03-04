import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Blog } from 'src/app/shared/models/blog';
import { BlogsService } from '../blogs.service';

@Component({
  selector: 'app-add-blog',
  templateUrl: './add-blog.component.html',
  styleUrls: ['./add-blog.component.css']
})
export class AddBlogComponent implements OnInit {
  blogForm: FormGroup;
  model: Blog;

  constructor(public blogsService: BlogsService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.createBlog();
  }

  createBlog() {
    this.blogForm = this.fb.group({
      title: [null, [Validators.required]],
      blogContent: ['', [Validators.required,
        Validators.minLength(10), Validators.maxLength(3000)]],
      picture: ''
    });
  }

  onSubmit() {
    this.blogsService.createBlog(this.blogForm.value).subscribe(() => {
      this.router.navigateByUrl('blogs');
    },
    error => {
      console.log(error);
    });
  }

  onImageSelected(image){
    this.blogForm.get('picture').setValue(image);
  }

}
