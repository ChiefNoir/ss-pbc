import ReactDOM from "react-dom/client";
import "./index.scss";
import { Footer, Header, Navigation } from "./features";
import { BrowserRouter } from "react-router-dom";
import { AppRouter } from "./AppRouter";
import { ThemeProvider, createTheme } from "@mui/material/styles";
import { store } from "./storage";
import { Provider } from "react-redux";
import { fetchFingerprint } from "./storage/slices/fingerprintSlice";

const darkTheme = createTheme({
  palette: {
    mode: "dark"
  }
});

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);

store.dispatch(fetchFingerprint());

root.render(
  <Provider store={store}>
    <ThemeProvider theme={darkTheme}>
      <BrowserRouter>
        <Header/>
        <Navigation />
        <AppRouter />
        <Footer/>
      </BrowserRouter>
    </ThemeProvider>
  </Provider>
);
