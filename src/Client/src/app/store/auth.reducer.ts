import { createReducer, on } from '@ngrx/store';
import {clearToken, setToken} from './auth.actions';
import {initialAuthState} from './auth.store';

export const authReducer = createReducer(
  initialAuthState,
  on(setToken, (state, { accessToken }) => ({
    ...state,
    accessToken,
  })),
  on(clearToken, (state) => ({
    ...state,
    token: null,
  }))
);
