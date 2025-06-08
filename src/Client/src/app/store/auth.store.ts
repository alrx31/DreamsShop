export interface AuthState{
  accessToken: string | null;
}

export const initialAuthState: AuthState = {
  accessToken: null,
}
