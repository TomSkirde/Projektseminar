import React from 'react';

const ParseModelButton: React.FC = () => {

    const callApi = async () => {
        try {
            const response = await fetch('/api/Main/LoadModel', {
                method: "POST"
            });
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
        } catch (error) {
            console.error('API call failed:', error);
        }
    };

    return (
        <div className="p-4">
            <button onClick={callApi}>
                Call API
            </button>
        </div>
    )

}

export default ParseModelButton;
