import React, { useState } from "react";
import { useWorkflow } from '../context/WorkflowContext';

const GetJsonsButton: React.FC = () => {
    const { setModels, setSelectedModel } = useWorkflow();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    async function loadModels() {
        setLoading(true);
        setError(null);

        try {
            const res = await fetch('/api/Main/GetJsons', {
                headers: { Accept: 'application/json' }
            });

            if (!res.ok) {
                throw new Error(`HTTP ${res.status}`);
            }

            const payload = await res.json() as unknown;
            const data = typeof payload === 'string' ? JSON.parse(payload) : payload;

            setModels(data);
        } catch (err) {
            setError((err as Error).message);
        } finally {
            setLoading(false);
        }
    }

    const handleModelSelect = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const selectedPath = event.target.value;
        const selectedModel = selectedPath ? { Name: event.target.options[event.target.selectedIndex].text, Path: selectedPath } : null;
        setSelectedModel(selectedModel);
    };

    return (
        <button
            onClick={loadModels}
            disabled={loading}
        >
            {loading ? 'Loading...' : 'Load Models'}
        </button>
    );
};

export default GetJsonsButton;