import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AccountService } from '../account/account.service';
import { ServiceIncluded, ServiceIncludedCreateEdit } from '../shared/models/birthdays/serviceincluded';
import { UserParams } from '../shared/models/myparams';
import { IPaginationFofServicesIncluded } from '../shared/models/pagination';
import { User } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class ServicesIncludedService {
  baseUrl = environment.apiUrl;
  user: User;
  userParams: UserParams;

  constructor(private http: HttpClient,
              private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take (1)).subscribe(user => {
      this.user = user;
      this.userParams = new UserParams(user);
});
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

  getServicesIncluded(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationFofServicesIncluded>(this.baseUrl + 'birthdays/servicesincluded', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getServiceIncludedById(id: number) {
    return this.http.get<ServiceIncluded>(this.baseUrl + 'birthdays/servicesincluded/' + id);
  }

  createServiceIncluded(servicesIncluded: ServiceIncludedCreateEdit) {
    const formData = this.BuildFormData(servicesIncluded);
    return this.http.post(this.baseUrl + 'birthdays/servicesincluded', formData);
  }

  updateServiceIncluded(id: number, serviceIncluded: ServiceIncludedCreateEdit){
    const formData = this.BuildFormData(serviceIncluded);
    return this.http.put(this.baseUrl + 'birthdays/servicesincluded/' + id, formData);
  }

  private BuildFormData(serviceIncluded: ServiceIncludedCreateEdit): FormData {
    const formData = new FormData();
    if (serviceIncluded.id) {
    formData.append('id', JSON.stringify(serviceIncluded.id));
    }
    if (serviceIncluded.name){
    formData.append('name', serviceIncluded.name);
    }
    if (serviceIncluded.description){
    formData.append('description', serviceIncluded.description);
    }
    if (serviceIncluded.picture){
      formData.append('picture', serviceIncluded.picture);
    }
    if (serviceIncluded.videoClip){
      formData.append('videoClip', serviceIncluded.videoClip);
    }
    return formData;
  }

}





