import {Component} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule} from '@angular/forms';
import {Auth} from '../../../services/auth/auth';
import {Router} from '@angular/router';
import {environment} from '../../../environment/environment';

@Component({
  selector: 'app-login',
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  loginForm: FormGroup;

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
        alert(err.message);
        console.error(err);
      }
    });
  }

  routeToRegister(){
    this.router.navigate(['/reg']);
  }
}
