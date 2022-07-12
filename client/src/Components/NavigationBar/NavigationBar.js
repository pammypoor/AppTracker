import React, { useEffect, useState } from "react";
import jwt_decode from "jwt-decode";
import { UseDarkMode } from "../Themes/UseDarkMode";
import {ThemeProvider} from "styled-components";
import { GLobalStyles } from "../GlobalStyles/GobalStyles";
import { lightTheme, darkTheme } from "../Themes/Themes";
import ThemeToggle from "../ThemeToggle/ThemeToggle";
import './NavigationBar.css';
import { toHaveAccessibleName } from "@testing-library/jest-dom/dist/matchers";
import { render } from "@testing-library/react";
import { Link } from "react-router-dom";
import { FiMenu, FiX } from 'react-icons/fi';

function NavigationBar() {
    const [menuOpen, setMenuOpen] = useState(false);
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

    const toggleMenu = () => {
        setMenuOpen(!menuOpen);
    }

    const renderNav = (
        <div className = "authenticated-navbar-container">
            <nav className = "authenticated-navbar">
                <Link to="/" className="nav-logo" onClick = {toggleMenu}>
                    Logo
                </Link>
                <div onClick = {toggleMenu} className="nav-icon">
                    {menuOpen ? <FiX/> : <FiMenu/>}
                </div>
                <ul className = {menuOpen ? 'nav-links active': 'nav-links'}>
                    <li className="nav-theme-toggle">
                        {renderToggle}
                    </li>
                    <li className="nav-item">
                        <a href="" className="nav-link" onClick={toggleMenu}>
                            About
                        </a>
                    </li>
                    <li className="nav-item">
                        <a href="" className="nav-link" onClick={toggleMenu}>
                            Sign Up
                        </a>
                    </li>
                    <li className="nav-item">
                        <a href="\SignIn"   className="nav-link" onClick={toggleMenu}>
                            Sign In
                        </a>
                    </li>
                </ul>
                
            </nav>
        </div>
    )

    return (
        <div className="navbar-wrapper"> 
            {renderNav}
        </div>
    )
}

export default NavigationBar;