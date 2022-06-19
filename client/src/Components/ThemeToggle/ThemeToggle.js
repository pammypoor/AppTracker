import React from "react";
import {func, string } from 'prop-types';
import styled from "styled-components";
import { FaToggleOff, FaToggleOn} from 'react-icons/fa';

const Button = styled.button`
    background: transparent;
    border: none !important;
`
const ThemeToggle = ({theme, toggleTheme}) => {
    const toggle = () => {

    }

    return (
        <Button onClick={toggleTheme}>
            {theme === 'light' ? <FaToggleOn/> : <FaToggleOff/>}
        </Button>
    );
};

ThemeToggle.prototypes = {
    theme: string.isRequired,
    toggleTheme: func.isRequired,
}

export default ThemeToggle;