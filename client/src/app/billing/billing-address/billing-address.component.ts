import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { Address } from 'src/app/shared/models/address';

@Component({
  selector: 'app-billing-address',
  templateUrl: './billing-address.component.html',
  styleUrls: ['./billing-address.component.css']
})
export class BillingAddressComponent implements OnInit {
  @Input() billingForm: FormGroup;
  countryList = [];

  constructor(private accountService: AccountService,
              private toastr: ToastrService) { }

  ngOnInit(): void {
    this.accountService.getCountries()
    .subscribe(res => this.countryList = res as []);
  }

  updateCustomerAddress() {
    this.accountService.updateCustomerAddress(this.billingForm.get('addressForm').value)
      .subscribe((address: Address) => {
        this.toastr.success('Address saved');
        this.billingForm.get('addressForm').reset(address);
      }, error => {
        this.toastr.error(error.message);
        console.log(error);
      });
  }

}
