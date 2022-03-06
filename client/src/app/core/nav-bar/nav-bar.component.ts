import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { BasketService } from 'src/app/basket/basket.service';
import { BirthdayPackagesService } from 'src/app/birthday-packages/birthday-packages.service';
import { Basket } from 'src/app/shared/models/basket';
import { ExternalAuth } from 'src/app/shared/models/externalauth';
import { User } from 'src/app/shared/models/user';
import { SocialUser } from 'angularx-social-login';


@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {
  currentUser$: Observable<User>;
  basket$: Observable<Basket>;
  isCollapsed = true;
  user: User;
  locationList = [];

  constructor(public accountService: AccountService,
              private birthdayPackagesService: BirthdayPackagesService,
              private basketService: BasketService,
              private router: Router)
{
}

ngOnInit(): void {
this.currentUser$ = this.accountService.currentUser$;
this.basket$ = this.basketService.basket$;

this.birthdayPackagesService.getLocations()
.subscribe(res => this.locationList = res as []);
}

public externalLogin = () => {
  this.accountService.signInWithGoogle()
  .then(res => {
    const user: SocialUser = { ...res };
    console.log(user);
    const externalAuth: ExternalAuth = {
      provider: user.provider,
      idToken: user.idToken
    };
    this.validateExternalAuth(externalAuth);
  }, error => console.log(error));
}

private validateExternalAuth(externalAuth: ExternalAuth) {
  this.accountService.externalLogin(externalAuth)
    .subscribe(res => {
      this.router.navigateByUrl('/');
    },
    error => {
      console.log(error);
      this.accountService.signOutExternal();
    });
}


logout() {
  this.accountService.logout();
}

}
