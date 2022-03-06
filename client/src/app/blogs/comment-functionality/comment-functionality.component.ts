import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { BlogCommentClass, BlogCommentClientOnlyClass } from 'src/app/shared/models/blogcomment';
import { User } from 'src/app/shared/models/user';
import { BlogsService } from '../blogs.service';

@Component({
  selector: 'app-comment-functionality',
  templateUrl: './comment-functionality.component.html',
  styleUrls: ['./comment-functionality.component.css']
})
export class CommentFunctionalityComponent implements OnInit {
  @Input() blogId: number;
  user: User;
  currentUser$: Observable<User>;
  standAloneComment: BlogCommentClientOnlyClass;
  blogComments: BlogCommentClass[];
  blogCommentsClientOnly: BlogCommentClientOnlyClass[];

  constructor(private blogsService: BlogsService,
              public accountService: AccountService)
              { this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user); }

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;
    this.blogsService.getAllBlogComments(this.blogId).subscribe(blogComments => {
      // now we have to put blog comments in the proper order, we have to use recursive
            if (this.user) {
              // if it is loggedin
              // we are initialising first comment, which will be an empty comment and thats
              // where we are going to allow the person to leave a comment
              this.initComment(this.user.displayName);
            }
            // this is equal to the comments we are recieving from the api
            this.blogComments = blogComments;
            this.blogCommentsClientOnly = [];
          // first we are going through each comment to establish whether
          // it has or it has not replies
            for (let i = 0; i < this.blogComments.length; i++) {
              // if the comment we are looping through doesn't have parentblogcommentid, so
              // this means that this comment is stand alone comment
              // sada želiš pronaći odgovore na taj standalone comment
              if (!this.blogComments[i].parentBlogCommentId) {
                this.findCommentReplies(this.blogCommentsClientOnly, i);
              }
            }
          });
  }

  initComment(username: string) {
    this.standAloneComment = {
      // nema ovo jer nije odgovor na nečiji komentar, u template pišeš inicijalni komentar
      parentBlogCommentId: null,
      commentContent: '',
      blogId: this.blogId,
      // creating comment, that's why it is -1
      id: -1,
      username: username,
      publishedOn: null,
      updatedOn: null,
      // ne može biti editable kad je tek stvoren
      isEditable: false,
      deleteConfirm: false,
      isReplying: false,
      comments: []
    };
  }

  findCommentReplies(blogCommentsClientOnly: BlogCommentClientOnlyClass[], index: number) {
    // we are getting the first comment
    const firstElement = this.blogComments[index];
    // we are initialising new array when we enter this method
    const newComments: BlogCommentClientOnlyClass[] = [];

    const commentViewModel: BlogCommentClientOnlyClass = {
      // this will be null if parentblogcommentid is nothing
      parentBlogCommentId: firstElement.parentBlogCommentId || null,
      commentContent: firstElement.commentContent,
      id: firstElement.id,
      blogId: firstElement.blogId,
      username: firstElement.username,
      publishedOn: firstElement.publishedOn,
      updatedOn: firstElement.updatedOn,
      isEditable: false,
      deleteConfirm: false,
      isReplying: false,
      // ovo su novi commentsi koje smo gore u ovoj metodi inicializirali
      comments: newComments
    };
     // we are pushing this into a list
     // so we would have the correct order in the template
     // this will be the list we display
    blogCommentsClientOnly.push(commentViewModel);

    // this is where the recursion occurs
    // we are once again going through all the comments
    for (let i = 0; i < this.blogComments.length; i++) {
      // this means that blogcomment[i] is the reply of firstelement, lijevi je reply od desnog
      if (this.blogComments[i].parentBlogCommentId === firstElement.id) {
        // we are calling this function again (recursive), with different parameters
        // here we are searching for replies of the reply (zato koristimo recursive), and we
        // are doing this until there are no replies
        this.findCommentReplies(newComments, i);
      }
    }
  }

  onCommentSaved(blogComment: BlogCommentClass) {
    const commentViewModel: BlogCommentClientOnlyClass = {
      parentBlogCommentId: blogComment.parentBlogCommentId,
      commentContent: blogComment.commentContent,
      blogId: blogComment.blogId,
      id: blogComment.id,
      username: blogComment.username,
      publishedOn: blogComment.publishedOn,
      updatedOn: blogComment.updatedOn,
      isEditable: false,
      deleteConfirm: false,
      isReplying: false,
      comments: []
    };
  // hover over unshift, inserts new element at the start
  // this will eventually be assigned to blogcommentviewmodel via eventemitter in comment-box,
  // pogledaj to tamo, imaš objašnjenje
  // the visual effect of this is - you have a commentbox, you write a comment, and that comment
  // appears on the list of comments
    this.blogCommentsClientOnly.unshift(commentViewModel);
  }


}







