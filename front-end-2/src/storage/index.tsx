import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";
import { saveIdentity, deleteIdentity } from "./slices/identitySlice";
import { store, AppDispatch, RootState } from "./store";

export { saveIdentity, deleteIdentity };
export { store };

// hooks
export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
