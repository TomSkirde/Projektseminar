import React, { useState } from 'react';
import { useWorkflow } from '../context/WorkflowContext';

const ParseModelButton: React.FC = () => {
    const { selectedModel, setUploadStatus } = useWorkflow();
    const [loading, setLoading] = useState(false);

    const parseModel = async () => {
        if (!selectedModel) return;
        
        setLoading(true);
        try {
            const response = await fetch('/api/Main/LoadModel', {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ OwlPath: selectedModel.Path })
            });
            
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            setUploadStatus('Model parsed successfully');
        } catch (error) {
            console.error('API call failed:', error);
            setUploadStatus('Failed to parse model');
        } finally {
            setLoading(false);
        }
    };

    return (
        <button
            onClick={parseModel}
            disabled={!selectedModel || loading}
        >
            {loading ? 'Parsing...' : 'Parse Model'}
        </button>
    );
}

export default ParseModelButton;
