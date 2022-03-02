import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BirthdayPackagesService } from '../birthday-packages.service';

@Component({
  selector: 'app-send-message',
  templateUrl: './send-message.component.html',
  styleUrls: ['./send-message.component.css']
})
export class SendMessageComponent implements OnInit {
  messageForm: FormGroup;

  constructor(public birthdayPackagesService: BirthdayPackagesService,
              private toastr: ToastrService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.createBirthdayForm();
  }

  createBirthdayForm() {
    this.messageForm = this.fb.group({
      firstLastName: [null, [Validators.required]],
      messageContent: [null, [Validators.required]],
      email: [null, [Validators.required]],
      phone: [null, [Validators.required]]
    });
  }

  onSubmit() {
    this.birthdayPackagesService.createMessage(this.messageForm.value).subscribe(() => {
      this.messageForm.reset();
      this.toastr.success('Your message has been sent!');
    },
    error => {
      console.log(error);
    });
  }

}
