import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BlogCommentClass, BlogCommentClientOnlyClass, BlogCommentCreateEditClass } from 'src/app/shared/models/blogcomment';
import { BlogsService } from '../blogs.service';

@Component({
  selector: 'app-comment-box',
  templateUrl: './comment-box.component.html',
  styleUrls: ['./comment-box.component.css']
})
export class CommentBoxComponent implements OnInit {
  @Input() comment: BlogCommentClientOnlyClass;
  @Output() commentSaved = new EventEmitter<BlogCommentClass>();

  @ViewChild('commentForm') commentForm: NgForm;

  constructor(private blogsService: BlogsService,
              private toastr: ToastrService
  ) { }

  ngOnInit(): void {
  }

  resetComment() {
    this.commentForm.reset();
  }

  onSubmit() {
    // this is how we will save the comment on the backend
    const blogCommentCreate: BlogCommentCreateEditClass = {
      id: this.comment.id,
      parentBlogCommentId: this.comment.parentBlogCommentId,
      blogId: this.comment.blogId,
      commentContent: this.comment.commentContent
    };

    this.blogsService.upsertBlogComment(blogCommentCreate).subscribe(blogComment => {
      // order matters, first toast, then reset and eventemitter
      // if you emit and reset you will send null content value
      this.toastr.info('Comment saved.');
      this.resetComment();
      // we are shooting the value back up, it will emit the value of blogcomment
      // the moment we save the comment on submit, when we get back the saved comment, we
      // are going to emit that value back up via commentsaved method
      // commentSaved is hooked up on onCommentSaved from the comment-system
      // onCommentSaved will grab that saved comment and assign that to our commentviewmodel
      this.commentSaved.emit(blogComment);
    });
  }
}
