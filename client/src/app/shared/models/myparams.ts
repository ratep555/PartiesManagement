import { User } from './user';

export class MyParams {
    query: string;
    page = 1;
    pageCount = 12;
}

export class UserParams {
    query: string;
    page = 1;
    pageCount = 12;
    displayName: string;

    constructor(user: User) {
        this.displayName = user.displayName;
   }
}
