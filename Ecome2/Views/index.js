const express = require('express');
const session = require('express-session');
const passport = require('passport');
const GoogleStrategy = require('passport-google-oauth20').Strategy;
const mysql = require('mysql2');

// Veritabanı bağlantısı
const connection = mysql.createConnection({
    host: 'DESKTOP-KKR16AR',
    user: 'DESKTOP-KKR16AR\hamidoffa',
    password: '',
    database: 'Ecome2'
});

connection.connect(err => {
    if (err) {
        console.error('Error connecting to the database:', err);
        return;
    }
    console.log('Connected to the database');
});

// Kullanıcıları serileştirme ve deserialize etme
passport.serializeUser((user, done) => {
    done(null, user.id);
});

passport.deserializeUser((id, done) => {
    connection.query('SELECT * FROM users WHERE id = ?', [id], (err, results) => {
        if (err) {
            return done(err);
        }
        done(null, results[0]);
    });
});

// Google Strategy yapılandırması
passport.use(new GoogleStrategy({
    clientID: "791460801499-mt8lnunni3kc5tuum0i7csdj87c71vla.apps.googleusercontent.com"
    clientSecret: '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'
    callbackURL: 'https://localhost:7085/'
    scope: ['profile', 'email']
},
    (accessToken, refreshToken, profile, done) => {
        const email = profile.emails[0].value;
        connection.query('SELECT * FROM users WHERE email = ?', [email], (err, results) => {
            if (err) {
                return done(err);
            }
            if (results.length === 0) {
                const user = { email: email, name: profile.displayName, google_id: profile.id };
                connection.query('INSERT INTO users SET ?', user, (err, result) => {
                    if (err) {
                        return done(err);
                    }
                    user.id = result.insertId;
                    return done(null, user);
                });
            } else {
                return done(null, results[0]);
            }
        });
    }
));

const app = express();

app.use(session({
    secret: 'your-secret-key',
    resave: false,
    saveUninitialized: true
}));

app.use(passport.initialize());
app.use(passport.session());

app.set('view engine', 'ejs');

// Giriş sayfası
app.get('/', (req, res) => {
    res.render('index', { user: req.user });
});

// Google ile giriş yapma
app.get('/auth/google',
    passport.authenticate('google', { scope: ['profile', 'email'] })
);

// Google giriş sonrası callback
app.get('/auth/google/callback',
    passport.authenticate('google', { failureRedirect: '/' }),
    (req, res) => {
        res.redirect('/');
    }
);

app.listen(3000, () => {
    console.log('Server is running on port 3000');
});
