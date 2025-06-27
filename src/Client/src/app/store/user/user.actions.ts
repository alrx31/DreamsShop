import {createAction, props} from '@ngrx/store';

export const setUserInfo = createAction(
  '[User] Set user info',
  props<{role: string | null}>()
);

export const clearUserInfo = createAction(
  '[User] Clear user info',
)
