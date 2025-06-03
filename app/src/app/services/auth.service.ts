import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, filter } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);

  public isAuthenticated$: Observable<boolean> = this.isAuthenticatedSubject.asObservable().pipe(filter((val): val is boolean => val !== null));

  constructor() {
    this.checkAuth();
  }

  async checkAuth(): Promise<boolean> {
    console.log("Checking auth");
    try {
      const res = await fetch('http://localhost:5137/api/auth/status', {
        credentials: 'include',
        method: 'GET'
      });

      if(!res.ok) {
        console.log("response not ok - fail");
         throw new Error('USER NOT AUTHENTICATED');
      } else {
        console.log("response good");
        const authenticated = res.ok;
        this.isAuthenticatedSubject.next(authenticated);
        console.log("auth");
        return authenticated;
      }
    } catch {
      this.isAuthenticatedSubject.next(false);
      return false;
    }
  }

  async login(username: string, password: string): Promise<boolean> {
    const user = username;
    const pass = password;
    console.log("login() running");
    try {
      console.log("fetching");
      const res = await fetch('http://localhost:5137/api/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials:'include',
        body: JSON.stringify({ username: user, password: pass })
      });
      if(!res.ok) { 
        console.log("fail");
        throw new Error('USER NOT FOUND');
      }
      console.log("success");
      this.isAuthenticatedSubject.next(true);
      return true;
      } catch(err) {
        console.log(err);
        this.isAuthenticatedSubject.next(false);
        return false;
      }
  }

  async register(username: string, password: string): Promise<boolean> {
    const user = username;
    const pass = password;
    console.log("login() running");
    try {
      console.log("fetching");
      const res = await fetch('http://localhost:5137/api/auth/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials:'include',
        body: JSON.stringify({ id: 0, username: user, password: pass })
      });
      if(!res.ok) { 
        console.log("fail");
        throw new Error('USER NOT FOUND');
      }
      console.log("success");
      this.isAuthenticatedSubject.next(true);
      return true;
      } catch(err) {
        console.log(err);
        this.isAuthenticatedSubject.next(false);
        return false;
      }
  }
}