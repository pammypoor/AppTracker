import React, { useEffect, useState } from "react";
import jwt_decode from "jwt-decode";
import { UseDarkMode } from "../Themes/UseDarkMode";
import {ThemeProvider} from "styled-components";
import { GLobalStyles } from "../GlobalStyles/GobalStyles";
import { lightTheme, darkTheme } from "../Themes/Themes";
import ThemeToggle from "../ThemeToggle/ThemeToggle";

function NavigationBar() {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [theme, themeToggler] = UseDarkMode();

    const themeMode = theme === 'light' ? lightTheme : darkTheme;

    const checkToken = () => {
        const token = sessionStorage.getItem('authorization');
        if(token != null){
            const decoded = jwt_decode(token);
            setIsAuthenticated(true);
        }      
    }

    const renderToggle = (
        <ThemeProvider theme = {themeMode}>
            <>
            <GLobalStyles/>
            <ThemeToggle theme = {theme} toggleTheme={themeToggler}/>
            </>
        </ThemeProvider>
    )

    const renderNav = (
        <nav className = "authenticated-navbar-container">
           <ul className = "nav-links">
                <li className="logo"><a href="/" >LOGO</a></li>
                <li className="nav-toggle">{renderToggle}</li> 
                <li className="nav-link">About</li>
                <li className="nav-link">Sign Up</li>
                <li className="nav-link">Sign In</li>
            </ul>
        </nav>
    );

    useEffect(() => {
        checkToken();
    }, [])
   
    return (
        <div className="navbar-wrapper"> 
            {renderNav}
        </div>
    );
}

export default NavigationBar;