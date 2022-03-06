import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { BlogCommentClass, BlogCommentClientOnlyClass } from 'src/app/shared/models/blogcomment';
import { User } from 'src/app/shared/models/user';
import { BlogsService } from '../blogs.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnInit {
  @Input() comments: BlogCommentClientOnlyClass[];
  currentUser$: Observable<User>;
  user: User;


  constructor(public accountService: AccountService,
              private toastr: ToastrService,
              private blogsService: BlogsService)
              { this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user); }

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;

  }

  editComment(comment: BlogCommentClientOnlyClass) {
    comment.isEditable = true;
  }

  showDeleteConfirm(comment: BlogCommentClientOnlyClass) {
    comment.deleteConfirm = true;
  }

  cancelDeleteConfirm(comment: BlogCommentClientOnlyClass) {
    comment.deleteConfirm = false;
  }

  deleteConfirm(comment: BlogCommentClientOnlyClass, comments: BlogCommentClientOnlyClass[]) {
    this.blogsService.deleteBlogComment(comment.id).subscribe(() => {

      let index = 0;

      for (let i = 0; i < comments.length; i++) {
        if (comments[i].id === comment.id) {
          index = i;
        }
      }
     // if there is an index
      if (index > -1) {
        comments.splice(index, 1);
      }

      this.toastr.info('Blog comment deleted.');
    });
  }

  replyComment(comment: BlogCommentClientOnlyClass) {
    const replyComment: BlogCommentClientOnlyClass = {
      parentBlogCommentId: comment.id,
      commentContent: '',
      blogId: comment.blogId,
      // this is basically a new comment, so it doesn't have an id
      id: -1,
      username: this.user.displayName,
      // when we send this to backend, dates will be calculated, but we can do this
      publishedOn: new Date(),
      updatedOn: new Date(),
      // it is not going to be editable since it is newly created
      isEditable: false,
      deleteConfirm: false,
      isReplying: true,
      // meaning this comment does not have replies since it is newly created
      comments: []
    };
    // we are adding comment to replies of this comment - to the end (push metoda!)
    // this will be picked up by comment-system, iterated and shown recursively which
    // comment belongs to which
    comment.comments.push(replyComment);
  }

  onCommentSaved(blogComment: BlogCommentClass, comment: BlogCommentClientOnlyClass) {
    comment.id = blogComment.id;
    comment.parentBlogCommentId = blogComment.parentBlogCommentId;
    comment.blogId = blogComment.blogId;
    comment.commentContent = blogComment.commentContent;
    comment.publishedOn = blogComment.publishedOn;
    comment.updatedOn = blogComment.updatedOn;
    comment.username = blogComment.username;
    comment.isEditable = false;
    comment.isReplying = false;
  }
}
