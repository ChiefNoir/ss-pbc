import {
  CanActivate,
  ActivatedRouteSnapshot,
  Router
} from '@angular/router';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { StorageService } from './storage.service';

// @ts-ignore
import jwt_decode from 'jwt-decode';



@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {

  public validating$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    false
  );

  public constructor(
    private router: Router,
    private storageService: StorageService
  ) {

  }


  public isLoggedIn(): boolean
  {
    return this.getTokenData() !== null;
  }

  public canActivate(route: ActivatedRouteSnapshot): boolean
  {
    const tokenData = this.getTokenData();
    if (tokenData === null)
    {
      this.router.navigate(['/login']);
      return false;
    }

    const expectedRoles = route.data.expectedRoles as string[];
    if (expectedRoles.some(x => x === tokenData.role))
    {
      return true;
    }

    this.router.navigate(['/login']);
    return false;
  }

  public logoutComplete(): void {
    this.storageService.removeToken();
  }

  public canSee(routerLink: string): boolean {
    const tokenData = this.getTokenData();

    if (tokenData === null) {
      return false;
    }

    switch (routerLink) {
      case '/admin':
        {
          return ['admin', 'demo'].some(x => x === tokenData.role);
        }
        case '/admin/editor/introduction':
          {
            return ['admin', 'demo'].some(x => x === tokenData.role);
          }
        case '/admin/editor/projects':
          {
            return ['admin', 'demo'].some(x => x === tokenData.role);
          }
        case '/admin/editor/categories':
            {
              return ['admin', 'demo'].some(x => x === tokenData.role);
            }
        case '/admin/editor/accounts':
              {
                return ['admin'].some(x => x === tokenData.role);
              }
    }

    return false;
  }

  private getTokenData(): TokenData
  {
    const token = this.storageService.getToken();
    if (!token) { return null; }

    const tokenPayload = jwt_decode(token);
    if (Date.now() >= tokenPayload.exp * 1000 ) {
      this.storageService.removeToken();
      return null;
    }

    const role = tokenPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    const name = tokenPayload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];

    return new TokenData(role, name);
  }


}
class TokenData
{
  constructor(role: string, name: string) {
    this.role = role;
    this.name = name;
  }
  public role: string;
  public name: string;
}