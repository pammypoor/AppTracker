import axios from "axios";
import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

import Home from "./Pages/Home/Home";
import SignInUpForm from "./Components/Forms/SignInUp/SignInUpForm";
import Portal from "./Pages/Portal/Portal";
import NewApplication from "./Pages/NewApplication/NewApplication";
import Settings from "./Pages/Settings/Settings";

class App extends React.Component {
  constructor(props){
    super(props);
    axios.defaults.headers.common['Authorization'] = sessionStorage.getItem('authorization');
  }

  render() {
    return (
      <div className = "App"> 
        <Router>
          <header></header>
          <Routes>
            <Route path = "/" element = {<Home />}/>
            <Route path = "/SignIn" element = {<SignInUpForm />}/>
            <Route path = "/Portal" element = {<Portal/>}/>
            <Route path = "/NewApplication" element = {<NewApplication/>}/>
            <Route path = "/Settings" element = {<Settings/>}/>
          </Routes>
        </Router>
      </div>
    );
  }
}

export default App;