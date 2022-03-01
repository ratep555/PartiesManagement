import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { LocationCreateEdit } from 'src/app/shared/models/location';
import { UserParams } from 'src/app/shared/models/myparams';
import { IPaginationForLocations } from 'src/app/shared/models/pagination';
import { User } from 'src/app/shared/models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LocationsService {
  baseUrl = environment.apiUrl;
  user: User;
  userParams: UserParams;
  formData: LocationCreateEdit = new LocationCreateEdit();


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

  getLocations(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForLocations>(this.baseUrl + 'birthdays/locations', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  createLocation(location: LocationCreateEdit) {
    return this.http.post(this.baseUrl + 'birthdays/locations', location);
  }

  updateLocation(id: number, location: LocationCreateEdit){
    return this.http.put(this.baseUrl + 'birthdays/locations/' + id, location);
  }

}






