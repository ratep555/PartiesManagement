export interface User {
    displayName: string;
    token: string;
    email: string;
    roles: string[];
    userId: number;
    lockoutEnd: Date;
}

