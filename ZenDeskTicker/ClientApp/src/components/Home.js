import React, { Component } from 'react';
import './Home.css';

export class Home extends Component {
    displayName = Home.name

    constructor(props) {
        super(props);
        this.state = {
            daysSinceSev1: [],
            loading: true
        };

        fetch('api/NewRelic/DaysSinceSev?severity=1')
            .then(response => response.json())
            .then(data => {
                this.setState({ daysSinceSev1: data, loading: false });
            });
    }

    static renderHighScore(score) {
        console.log(score);
        if (typeof(score) === "number") {
            return <div className="score-container">
                <p className="score-title">Current High Score: <span className="score-result">{score} days</span></p>
            </div>;
        }

        return <div></div>;
    }

    static renderDaysSinceCounter(DaysSinceSev) {
        var cardClass = 'card'
        if (DaysSinceSev.daysSinceSev < 1) {
            cardClass += ' severity-critical';
        }
        var statusClass = 'statusError';
        if (DaysSinceSev.status && DaysSinceSev.status === 'solved') {
            statusClass = 'statusSuccess';
        }
        let current_datetime = new Date(DaysSinceSev.ticketCreatedAt);
        let formatted_date = current_datetime.getUTCFullYear() + "-" + (current_datetime.getUTCMonth() + 1) + "-" + current_datetime.getUTCDate() + " " + current_datetime.getUTCHours() + ":" + current_datetime.getUTCMinutes() + ":" + current_datetime.getUTCSeconds();
        return (
            <div>
                <h1 className={cardClass}>
                    {DaysSinceSev.daysSinceSev}
                </h1>
                <div className='metaData'>
                    <h2 className={statusClass}>
                        <strong>Status</strong>: {DaysSinceSev.status}
                    </h2>
                    <h2>
                        {formatted_date}: {DaysSinceSev.ticketTitle}
                    </h2>
                    <p>
                        <strong>Summary</strong>: {DaysSinceSev.ticketSummary}
                    </p>
                    <p>
                        <strong>Investigative Steps</strong>: {DaysSinceSev.investigativeSteps}
                    </p>
                    <p>
                        <strong>Resolution</strong>: {DaysSinceSev.resolutionSummary}
                    </p>
                </div>
                {Home.renderHighScore(DaysSinceSev.currentHighScore)}
            </div>
        );
    }

    tick() {
        this.setState({
            daysSinceSev1: [],
            loading: true
        });

        fetch('api/NewRelic/DaysSinceSev?severity=1')
            .then(response => response.json())
            .then(data => {
                this.setState({ daysSinceSev1: data, loading: false });
            });
    }

    componentDidMount() {
        this.interval = setInterval(() => this.tick(), 10 * 60 * 1000); //10 min
    }

    componentWillUnmount() {
        clearInterval(this.interval);
    }

    render() {
        let contents = this.state.loading
            ? <p>
                <em>Loading...</em>
            </p>
            : Home.renderDaysSinceCounter(this.state.daysSinceSev1);

        return (
            <div>
                <h1>Days Since Sev 1</h1>
                {contents}
            </div>
        );
    }
}
