import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { User } from 'src/app/shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private accountService: AccountService,
              private toastr: ToastrService,
              private router: Router ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    let currentUser: User;
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => currentUser = user);
    if (currentUser) {
        return true;
    }
    this.router.navigate(['account/login'], { queryParams: { returnUrl: state.url } });
    return false;
}
}

