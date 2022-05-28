import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { SharedService } from '../shared.service';
import { users } from '../models/users';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})

export class LoginComponent implements OnInit {
  invalidLogin: boolean = false;
  diffPass: boolean = false;

  constructor(private router: Router, private service: SharedService) { }

  ngOnInit(): void { }

  login(form: NgForm) {
    var credentials: users = {
      uName: form.value.username,
      uPass: form.value.password,
      uRole: form.value.role
    }

    try {
      this.service.loginUser(credentials).subscribe(
        res => {
          const token = (<any>res).token;
          localStorage.setItem("lms_authenticate", token); // session storage can also be used
          this.invalidLogin = false;
          if (credentials.uRole == "admin") {
            this.router.navigate(["admin/available-books"]);
          } else {
            this.router.navigate(["student/available-books"]);
          }
          this.service.SnackBarSuccessMessage("Login Successful!");
        }, err => {
          this.invalidLogin = true;
          this.service.SnackBarErrorMessage(JSON.stringify(err.message));
        }
      )
    } catch (error: any) {
      this.service.SnackBarErrorMessage(JSON.stringify(error.message));
    }
  }

  register(form: NgForm) {
    if (form.value.password == form.value.con_password) {
      var details: users = {
        uName: form.value.username,
        uPass: form.value.password,
        uRole: 'student'
      };

      try {
        this.service.registerUser(details).subscribe((res: any) => {
          this.service.SnackBarSuccessMessage(res.toString());
        }, err => {
          this.service.SnackBarErrorMessage(JSON.stringify(err.message));
        });
      } catch (error: any) {
        this.service.SnackBarErrorMessage(JSON.stringify(error));
      }
    }
    else {
      this.diffPass = true;
    }
  }
}
