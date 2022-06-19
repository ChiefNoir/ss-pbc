import { configureStore } from "@reduxjs/toolkit";
import { identitySlice } from "./slices/identitySlice";

const store = configureStore({
  reducer: {
    identity: identitySlice.reducer
  }
});

export { store };

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>;

// Inferred type: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch;
