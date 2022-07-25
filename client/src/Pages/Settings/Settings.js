import React from "react";
import jwt_decode from "jwt-decode";  
import { Navigation } from "react-minimal-side-navigation";
import { FaIdCard } from "react-icons/fa";
import { useNavigate, useLocation } from "react-router-dom";

import "./Settings.css";
import NavigationBar from "../../Components/NavigationBar/NavigationBar";
import UserEditableProfile from "../../Components/Profile/UserEditableProfile";
import ProfileForm from "../../Components/Forms/ProfileForm/ProfileForm";

class Settings extends React.PureComponent {
    constructor(props){
        super(props); 
        this.token = sessionStorage.getItem('authorization');
        this.state = {
            
        }
    }

    componentDidMount() {
        this.checkToken();
    }

    checkToken = () => {
        const token = sessionStorage.getItem('authorization');
        if(token) {
            const decoded = jwt_decode(token);
            const tokenExpiration = decoded.tokenExpiration;
            const now = new Date();

            if(now.getTime() > decoded.exp * 1000){
                localStorage.removeItem('authorization');
                window.location = '/';
            }
        } else {
            // Token doesn't exist or not valid
            localStorage.removeItem('authorization');
            
        }
    }

    render() {
        return(
            <div className ="settings-wrapper">
                <div className="settings-navbar-wrapper">
                    <NavigationBar/>
                </div>
                <ProfileForm/>
            </div>
        );
    }
}

export default Settings;