import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { NgForm, Validators, FormBuilder } from '@angular/forms';
import { SharedService } from '../shared.service';
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
    const credentials = {
      'u_name': form.value.username,
      'u_pass': form.value.password,
      'u_role': form.value.role
    }

    this.service.loginUser(credentials).subscribe(
      res => {
        const token = (<any>res).Token;
        localStorage.setItem("lms_authenticate", token); // session storage can also be used
        this.invalidLogin = false;
        if (credentials.u_role == "admin") {
          this.router.navigate(["/admin/available-books"]);
        } else {
          this.router.navigate(["/student/available-books"]);
        }
        this.service.SnackBarMessage("Login Successful!", "Dismiss");
      }, err => {
        this.invalidLogin = true;
        this.service.SnackBarMessage(err.error.title, "Dismiss");
      }
    )
  }

  register(form: NgForm) {
    if(form.value.password == form.value.con_password) {
      const val = {
        'u_name': form.value.username,
        'u_pass': form.value.password,
        'u_role': 'student'
      };
      
      this.service.registerUser(val).subscribe((res) => {
        // alert(res.toString());
        this.service.SnackBarMessage(res.toString(), "Dismiss");
      });
    }
    else{
      this.diffPass = true;
    }
  }
}
