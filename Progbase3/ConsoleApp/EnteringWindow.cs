using System;
using System.Collections.Generic;
using Terminal.Gui;

public class EnteringWindow : Window
{
    private UserRepository userRepository;

    protected TextField loginInput;
    protected TextField passwordInput;
    public EnteringWindow()
    {
        this.Title = "Entering Window";

        int rightColumnX = 20;

        Label loginLbl = new Label(2, 2, "Login ");
        loginInput = new TextField("")
        {
            X = rightColumnX,
            Y = Pos.Top(loginLbl),
            Width = 28,
        };
        this.Add(loginLbl, loginInput);

        Label passwordLbl = new Label(2, 4, "Password: ");
        passwordInput = new TextField("")
        {
            X = rightColumnX,
            Y = Pos.Top(passwordLbl),
            Width = 28,
            Secret = true,
        };
        this.Add(passwordLbl, passwordInput);

        Button singIn = new Button(16, 7, "Sing In");
        singIn.Clicked += SingIn;

        Label invitingRegister = new Label(13, 11, "Create a new account");
        Button signUp = new Button(16, 13, "Sign Up");
        signUp.Clicked += SignUp;

        this.Add(singIn, invitingRegister, signUp);

        Button exitBtn = new Button(5, 16, "Exit");
        exitBtn.Clicked += ClickQuit;
        this.Add(exitBtn);
    }

    private void SingIn()
    {
        User user = new User()
        {
            username = loginInput.Text.ToString(),
            password = passwordInput.Text.ToString(),
        };
        User dbUser = Hashing.SignIn(user.username, user.password, userRepository);
        if (dbUser.username == "u")
        {
            MessageBox.ErrorQuery("LogIn error", "Wrong password", "Back");
            SetUser(user);
        }
        else if (dbUser.username == "")
        {
            MessageBox.ErrorQuery("LogIn error", "Wrong login", "Back");
        }
        else if (user.username == dbUser.username && Hashing.HashCode(user.password) == dbUser.password)
        {
            user = dbUser;
            MessageBox.Query("Enter", "enter", "back");
        }
    }

    public void SetUser(User user)
    {
        this.loginInput.Text = user.username;
    }

    private void SignUp()
    {
        SignUpDialog dialog = new SignUpDialog(userRepository);
        Application.Run(dialog);

        if (!dialog.canceled)
        {
            User user = dialog.GetUser();
            if (!(user == null))
            {
                long userId = userRepository.Insert(user);
                user.id = userId;
            }
            else
                SignUp();
        }
    }

    public void SetRepository(UserRepository repository)
    {
        this.userRepository = repository;
    }

    public void ClickQuit()
    {
        Application.RequestStop();
    }
}