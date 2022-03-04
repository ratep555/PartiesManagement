import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { Blog } from 'src/app/shared/models/blog';
import { BlogsService } from '../blogs.service';

@Component({
  selector: 'app-edit-blog',
  templateUrl: './edit-blog.component.html',
  styleUrls: ['./edit-blog.component.css']
})
export class EditBlogComponent implements OnInit {
  blogForm: FormGroup;
  id: number;
  blog: Blog;


  constructor(public blogsService: BlogsService,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.loadBlog();

    this.blogForm = this.fb.group({
      id: [this.id],
      title: [null, [Validators.required]],
      blogContent: ['', [Validators.required,
        Validators.minLength(10), Validators.maxLength(3000)]],
      picture: ''
    });

    this.blogsService.getBlogById(this.id)
    .pipe(first())
    .subscribe(x => this.blogForm.patchValue(x));
  }

  loadBlog() {
    return this.blogsService.getBlogById(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe(response => {
    this.blog = response;
    }, error => {
    console.log(error);
    });
    }


  onSubmit() {
    this.blogsService.updateBlog(this.id, this.blogForm.value).subscribe(() => {
    this.router.navigateByUrl('blogs');
        }, error => {
          console.log(error);
        });
      }

      onImageSelected(image){
        this.blogForm.get('picture').setValue(image);
      }

}
