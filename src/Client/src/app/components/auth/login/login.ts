import {Component} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule} from '@angular/forms';
import {Auth} from '../../../services/auth/auth';
import {Router} from '@angular/router';
import {environment} from '../../../environment/environment';
import { Loader } from "../../aditional/loader/loader";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [
    ReactiveFormsModule,
    CommonModule,
    Loader
],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  loginForm: FormGroup;
  errorMessage: string = '';
  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: Auth,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: [''],
      password: [''],
    });
  }


  onSubmit() {
    if (this.loginForm.invalid) {
      this.errorMessage = "Please fill in all fields correctly.";
      return;
    }
    this.isLoading = true;
    const { email, password } = this.loginForm.value;

    this.authService.login(email || '', password || '').subscribe({
      next: (response) => {
        const token = response.accessToken;
        const userData = response.userData;

        localStorage.setItem(environment.accessTokenName, token);
        localStorage.setItem('UserInfo', JSON.stringify(userData));

        this.router.navigate(['/']);
      },
      error: (err) => {
        this.errorMessage = "Invalid email or password";
        this.loginForm.reset();
        console.error(err);
      }
    });
    this.isLoading = false;
  }

  routeToRegister(){
    this.router.navigate(['/reg']);
  }
}
