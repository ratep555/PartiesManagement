import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { HomeComponent } from './home/home/home.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'webshop', loadChildren: () => import('./webshop/webshop.module').then(mod => mod.WebshopModule)},
  {path: 'birthdaypackages', loadChildren:
  () => import('./birthday-packages/birthday-packages.module').then(mod => mod.BirthdayPackagesModule)},
  {path: 'items', loadChildren: () => import('./items/items.module').then(mod => mod.ItemsModule)},
  {path: 'blogs', loadChildren: () => import('./blogs/blogs.module').then(mod => mod.BlogsModule)},
  {path: 'discounts', loadChildren: () => import('./admin/discounts/discounts.module').then(mod => mod.DiscountsModule)},
  {path: 'services', loadChildren:
   () => import('./services-included/services-included.module').then(mod => mod.ServicesIncludedModule)},
  {path: 'locations', loadChildren: () => import('./admin/locations/locations.module').then(mod => mod.LocationsModule)},
  {path: 'itemwarehouses', loadChildren:
  () => import('./admin/item-warehouses/item-warehouses.module').then(mod => mod.ItemWarehousesModule)},
  {path: 'basket', loadChildren: () => import('./basket/basket.module').then(mod => mod.BasketModule)},
  {path: 'billing', canActivate: [AuthGuard],
  loadChildren: () => import('./billing/billing.module').then(mod => mod.BillingModule)},
  {path: 'billing1', canActivate: [AuthGuard],
  loadChildren: () => import('./billing1/billing1.module').then(mod => mod.Billing1Module)},
  {path: 'orders', canActivate: [AuthGuard],
  loadChildren: () => import('./orders/orders.module').then(mod => mod.OrdersModule)},
  {path: 'account', loadChildren: () => import('./account/account.module').then(mod => mod.AccountModule)},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
