import {createAction, props} from '@ngrx/store'

export const setToken = createAction(
  '[Auth] SetToken',
  props<{accessToken: string}>()
);

export const clearToken = createAction('[Auth] ClearToken');
