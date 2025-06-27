import {createReducer, on} from '@ngrx/store';
import {clearUserInfo, setUserInfo} from './user.actions';
import {initialState} from './user.store';

export const userReducer = createReducer(
  initialState,
  on(setUserInfo, (state, {role}) =>({
    ...state,
    role,
  })),
  on(clearUserInfo, (state)=>({
    ...state,
    role: null,
  }))
);
