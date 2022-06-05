import ReactDOM from "react-dom/client";
import "./index.scss";
import { reportWebVitals } from "./reportWebVitals";
import { Footer, Header, Navigation } from "./features";
import { BrowserRouter } from "react-router-dom";
import { AppRouter } from "./AppRouter";
import { ThemeProvider, createTheme } from "@mui/material/styles";
import store from "./store/store";
import { Provider } from "react-redux";

const darkTheme = createTheme({
  palette: {
    mode: "dark"
  }
});

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);

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

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
