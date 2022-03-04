import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AccountService } from '../account/account.service';
import { Blog, BlogCreateEdit } from '../shared/models/blog';
import { MyParams, UserParams } from '../shared/models/myparams';
import { IPaginationForBlogs, PaginationForBlogs } from '../shared/models/pagination';
import { User } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class BlogsService {
  baseUrl = environment.apiUrl;
  user: User;
  userParams: UserParams;
  pagination = new PaginationForBlogs();
  myParams = new MyParams();


  constructor(private http: HttpClient,
              private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take (1)).subscribe(user => {
      this.user = user;
      this.userParams = new UserParams(user);
});
}

  getAllBlogs() {
  let params = new HttpParams();
  if (this.myParams.query) {
    params = params.append('query', this.myParams.query);
  }
  params = params.append('page', this.myParams.page.toString());
  params = params.append('pageCount', this.myParams.pageCount.toString());
  return this.http.get<IPaginationForBlogs>
  (this.baseUrl + 'birthdays/blogslist', { observe: 'response', params })
    .pipe(
      map(response => {
        this.pagination = response.body;
        return this.pagination;
      })
    );
}

setMyParams(params: MyParams) {
  this.myParams = params;
}

getMyParams() {
  return this.myParams;
}

  getUserParams() {
    return this.userParams;
  }

  setUserParams(params: UserParams) {
    this.userParams = params;
  }

  resetUserParams() {
    this.userParams = new UserParams(this.user);
    return this.userParams;
  }

  getBlogs(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForBlogs>(this.baseUrl + 'birthdays/blogs', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getBlogById(id: number) {
    return this.http.get<Blog>(this.baseUrl + 'birthdays/blogs/' + id);
  }

  createBlog(blog: BlogCreateEdit) {
    const formData = this.BuildFormData(blog);
    return this.http.post(this.baseUrl + 'birthdays/blogs', formData);
  }

  updateBlog(id: number, blog: BlogCreateEdit){
    const formData = this.BuildFormData(blog);
    return this.http.put(this.baseUrl + 'birthdays/blogs/' + id, formData);
  }

  private BuildFormData(blog: BlogCreateEdit): FormData {
    const formData = new FormData();
    if (blog.id) {
    formData.append('id', JSON.stringify(blog.id));
    }
    if (blog.title){
    formData.append('title', blog.title);
    }
    if (blog.blogContent){
    formData.append('blogContent', blog.blogContent);
    }
    if (blog.picture){
      formData.append('picture', blog.picture);
    }
    return formData;
  }

}
