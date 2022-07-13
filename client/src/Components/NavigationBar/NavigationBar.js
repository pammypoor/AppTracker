import React, { useEffect, useState } from "react";
import jwt_decode from "jwt-decode";
import { UseDarkMode } from "../Themes/UseDarkMode";
import {ThemeProvider} from "styled-components";
import { GLobalStyles } from "../GlobalStyles/GobalStyles";
import { lightTheme, darkTheme } from "../Themes/Themes";
import ThemeToggle from "../ThemeToggle/ThemeToggle";
import { Link } from "react-router-dom";
import { ContextMenu, ContextMenuTrigger, MenuItem, showMenu } from "react-contextmenu";
import { FiMenu, FiX } from 'react-icons/fi';

import './NavigationBar.css';

function NavigationBar() {
    const [menuOpen, setMenuOpen] = useState(false);
    const [profileData, setProfileData] = useState([]);
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [theme, themeToggler] = UseDarkMode();

    const themeMode = theme === 'light' ? lightTheme : darkTheme;

    const checkToken = () => {
        const token = sessionStorage.getItem('authorization');
        if(token != null){
            const decoded = jwt_decode(token);
            const now = new Date();
            if(now.getTime() > decoded.exp * 1000){
                sessionStorage.removeItem('authorization');
                window.location = '/';
            }
            setProfileData(decoded.email[0]);
            setIsAuthenticated(true);
        }      
    }

    useEffect(() => {
        checkToken();
    }, [])

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

    const renderProfileLetter = (e) => {
        const initial = profileData.toString().toUpperCase();
        return initial;
    }

    const renderNav = (
        <div className = "not-authenticated-navbar-container">
            <nav className = "not-authenticated-navbar">
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

    const handleLeftMouseClickProfile = (e) => {
        const x = window.innerWidth-10;
        const y = 80;
        showMenu({
            position: {x, y},
            id: "contextmenu"
        });
    }

    const handleSignOut = (e) => {
        e.preventDefault();
        sessionStorage.removeItem('authorization');
        window.location = "/";
    }

    const renderAuthenticatedNav = (
        <nav className = "authenticated-navbar-container">
            <Link to="/" className="nav-logo" onClick = {toggleMenu}>
                    Logo
                </Link>
            <ul className = "nav-links">
                <li className = "auth-nav-theme-toggle">
                    {renderToggle}
                </li>
                <li className= "auth-profile">
                    <div className="authenticated-navbar-profile">
                        <ContextMenuTrigger id="contextmenu">
                            <div className = "navbar-profile" onClick={handleLeftMouseClickProfile}>
                                {renderProfileLetter()}
                            </div>
                        </ContextMenuTrigger>
                        <ContextMenu id = "contextmenu" className="nav-context-menu">
                            <MenuItem>Settings</MenuItem>
                            <MenuItem onClick={handleSignOut}>Sign out</MenuItem>
                        </ContextMenu>
                    </div>
                </li>
            </ul>
        </nav>
    );

    return (
        <div className="navbar-container"> 
            {isAuthenticated ? <div>{renderAuthenticatedNav}</div>: <div>{renderNav}</div>}
        </div>
    )
}

export default NavigationBar;